package koserver.login.services;

import java.io.IOException;
import java.math.BigInteger;
import java.net.Socket;
import java.util.Date;

import org.apache.log4j.Logger;

import koserver.common.network.Packet;
import koserver.common.network.PacketListener;
import koserver.common.network.crypt.Cryptor;
import koserver.common.services.AbstractService;
import koserver.common.utils.ArrayUtils;
import koserver.login.configuration.AccountConfig;
import koserver.login.delegate.AccountDelegate;
import koserver.login.enums.AccountAuthority;
import koserver.login.models.Account;
import koserver.login.network.PacketHandler;
import static koserver.login.enums.LoginResult.*;

/**
 * @author Kurokami Servicio para controlar la conexion con un cliente.
 */
public class AccountService extends AbstractService implements Runnable {

	private static Logger log = Logger.getLogger(AccountService.class.getName());
	private Socket clientSocket;
	private BigInteger publicKey;
	private Cryptor cryptor;
	private PacketHandler packetHandler;
	private short packetSecuenceId;

	/**
	 * Servicio para controlar la conexion con un cliente.
	 * 
	 * @param clientSocket
	 *            Socket conectado al cliente.
	 */
	public AccountService(Socket clientSocket, PacketHandler packetHandler) {
		publicKey = null;
		this.clientSocket = clientSocket;
		this.packetHandler = packetHandler;
		packetSecuenceId = 0;
	}
	
	@Override
	public void onInit() {
		//log.info("AccountService started");
	}

	@Override
	public void onDestroy() {
		//log.info("AccountService stopped");
	}
	
	public void onAccountConnect(final Account account) {
        log.info("Account " + account.getLogin() + " connected succefully");
    }

	@Override
	public void run() {
		try {
			PacketListener reader = new PacketListener(clientSocket.getInputStream());
			packetHandler.Initialize(this);
			this.onInit();
			while (!clientSocket.isClosed()) {
				try {
					Packet packet = reader.waitForPacket();
					if (isCryptoEnabled()) {
						byte[] data = packet.getData();
						data = cryptor.EncryptionWithCRC32(data);
						if (data != null) {
							// TODO: Check packet SecuenceID and Checksum
							data = ArrayUtils.arraySubset(data, 4, data.length);
							packetSecuenceId++;

							packet = new Packet(ArrayUtils.byteArrayToShort(new byte[] { data[0] }, 0));
							packet.setData(data);
						}
					}
					packet.setData(ArrayUtils.arraySubset(packet.getData(), 1, packet.getData().length));
					packetHandler.handlePacket(packet);
				} catch (IOException e) {
					//log.info("Closed connection from " + clientSocket.getInetAddress().getHostAddress() + ":" + clientSocket.getPort());
					this.onDestroy();
					return;
				} catch (Exception e) {
					log.warn("Error on client service: " + e.getMessage());
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		this.onDestroy();
	}
	
	public int authorizeAccount(final String login, final String password) {
        log.info("Try authorizing " + login);
        Account account = AccountDelegate.readAccountModelByLogin(login);
        if (account == null && AccountConfig.ACCOUNT_AUTOCREATE) {
        	log.info("Account " + login + " tryed to login but this account isn't registered, creating account.");
            account = new Account();
            account.setLogin(login);
            account.setPassword(password);
            account.setAuthority(AccountAuthority.NORMAL);
            account.setCreation(new Date());
            account.setConnection(this);
            AccountDelegate.createAccountModel(account);
        }
        
        if(account == null){
        	log.info("Account " + login + " tryed to login but this account isn't registered.");
        	return AUTH_NOT_FOUND;
        }

        if (!account.getPassword().equals(password)) {
            log.info("Account " + login + " tryed to login with incorrect password.");
            return AUTH_INVALID;
        }

        if (account.isBanned()) {
            log.info("Banned account " + login + " tryed to login.");
            return AUTH_BANNED;
        }

        this.onAccountConnect(account);
        
        return AUTH_SUCCESS;
    }

	public boolean isCryptoEnabled() {
		return publicKey != null;
	}

	public Socket getSocket() {
		return clientSocket;
	}

	public void setPublicKey(BigInteger key) {
		publicKey = key;
		cryptor = new Cryptor(publicKey);
	}

	public BigInteger getPublicKey() {
		return publicKey;
	}

	public Cryptor getCryptor() {
		return cryptor;
	}

	public short getPacketSecuenceId() {
		return packetSecuenceId;
	}
}
