package koserver.common.network;

import koserver.common.services.AbstractService;


/**
 * @author Kurokami Clase utilizada para controlar paquetes.
 */
/**
 * @author Kurokami
 * 
 */
public interface PacketHandler {
	public void Initialize(AbstractService clientService); 
	/**
	 * Maneja un paquete en base al opcode.
	 * 
	 * @param packet
	 *            Instancia de la clase Packet que contiene los datos del
	 *            paquete.
	 */
	public void handlePacket(Packet packet);
}