using System;
using System.IO;
using System.Text;

namespace KnightOnline.Domain.Networking
{
    public class Packet
    {
        public ushort Opcode { get; } // Opcodes are often shorts
        private MemoryStream _stream;
        private BinaryWriter _writer; // Only used while building an outgoing packet
        private BinaryReader _reader; // Only used while reading an incoming packet

        // Constructor for creating an outgoing packet
        public Packet(ushort opcode)
        {
            Opcode = opcode;
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream, Encoding.UTF8, false); // Use UTF-8, keep stream open
            // Write opcode automatically if it's part of the payload in this protocol
            // For now, assuming opcode is meta-data and not part of the _stream itself initially.
            // If it is, then: WriteUShort(opcode); (or however it's serialized)
        }

        // Constructor for parsing an incoming packet (opcode typically read separately first)
        public Packet(ushort opcode, byte[] incomingData)
        {
            Opcode = opcode;
            _stream = new MemoryStream(incomingData);
            _reader = new BinaryReader(_stream, Encoding.UTF8, false);
        }

        // Get the full packet payload (e.g., for sending)
        public byte[] GetPayload()
        {
            return _stream.ToArray();
        }
        
        // Get the underlying stream's buffer (more advanced, careful with usage)
        public MemoryStream GetStream()
        {
            return _stream;
        }


        // --- Writing methods (for outgoing packets) ---
        // Ensure writer is available
        private BinaryWriter Writer
        {
            get
            {
                if (_writer == null)
                    throw new InvalidOperationException("Packet is not in write mode or already finalized.");
                return _writer;
            }
        }

        public void WriteByte(byte value) => Writer.Write(value);
        public void WriteSByte(sbyte value) => Writer.Write(value);
        public void WriteBool(bool value) => Writer.Write(value);
        public void WriteUShort(ushort value) => Writer.Write(value);
        public void WriteShort(short value) => Writer.Write(value);
        public void WriteUInt(uint value) => Writer.Write(value);
        public void WriteInt(int value) => Writer.Write(value);
        public void WriteULong(ulong value) => Writer.Write(value);
        public void WriteLong(long value) => Writer.Write(value);
        public void WriteFloat(float value) => Writer.Write(value);
        public void WriteDouble(double value) => Writer.Write(value);

        public void WriteBytes(byte[] buffer) => Writer.Write(buffer);
        public void WriteBytes(byte[] buffer, int index, int count) => Writer.Write(buffer, index, count);

        // For length-prefixed strings (common in network protocols)
        // Prefix with UShort length
        public void WriteString(string value)
        {
            if (value == null) value = string.Empty;
            byte[] stringBytes = Encoding.UTF8.GetBytes(value);
            WriteUShort((ushort)stringBytes.Length); // Length prefix
            Writer.Write(stringBytes);
        }
        
        // Writes a fixed-length string, padded or truncated
        public void WriteFixedString(string value, int fixedLength)
        {
            if (value == null) value = string.Empty;
            byte[] stringBytes = Encoding.UTF8.GetBytes(value);
            byte[] buffer = new byte[fixedLength]; // Initialized to zeros

            int bytesToCopy = Math.Min(stringBytes.Length, fixedLength);
            Array.Copy(stringBytes, buffer, bytesToCopy);
            
            Writer.Write(buffer);
        }


        // --- Reading methods (for incoming packets) ---
        // Ensure reader is available
        private BinaryReader Reader
        {
            get
            {
                if (_reader == null)
                    throw new InvalidOperationException("Packet is not in read mode.");
                return _reader;
            }
        }
        
        public long Position => _stream.Position;
        public long Length => _stream.Length;
        public long BytesRemaining => _stream.Length - _stream.Position;


        public byte ReadByte() => Reader.ReadByte();
        public sbyte ReadSByte() => Reader.ReadSByte();
        public bool ReadBool() => Reader.ReadBoolean();
        public ushort ReadUShort() => Reader.ReadUInt16();
        public short ReadShort() => Reader.ReadInt16();
        public uint ReadUInt() => Reader.ReadUInt32();
        public int ReadInt() => Reader.ReadInt32();
        public ulong ReadULong() => Reader.ReadUInt64();
        public long ReadLong() => Reader.ReadInt64();
        public float ReadFloat() => Reader.ReadSingle();
        public double ReadDouble() => Reader.ReadDouble();

        public byte[] ReadBytes(int count) => Reader.ReadBytes(count);

        // For length-prefixed strings (match WriteString)
        public string ReadString()
        {
            ushort length = ReadUShort(); // Read length prefix
            byte[] stringBytes = Reader.ReadBytes(length);
            return Encoding.UTF8.GetString(stringBytes);
        }
        
        // Reads a fixed-length string
        public string ReadFixedString(int fixedLength)
        {
            byte[] stringBytes = Reader.ReadBytes(fixedLength);
            // Trim null characters from the end, which are common for padding
            string value = Encoding.UTF8.GetString(stringBytes);
            return value.TrimEnd('\0');
        }

        // Dispose pattern if needed, especially if _stream needs explicit disposal
        // For MemoryStream, it's not strictly necessary if all operations are done before it goes out of scope.
        // However, if Packet instances are long-lived or passed around, implement IDisposable.
        // For now, assuming short-lived packet objects.
    }
}
