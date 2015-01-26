package koserver.common.network.config;

import java.net.InetSocketAddress;
import java.net.SocketAddress;


public class ServerConfig {

    private final String address;
    private final int port;
    
    public ServerConfig(final String address, final int port) {
        this.address = address;
        this.port = port;
    }

    public final int getPort() {
        return port;
    }

    public final String getAddress() {
        return this.address;
    }

    public final SocketAddress getInetSocketAddress() {
        SocketAddress inetSocketAddress = null;
        if ("*".equals(this.address)) {
            inetSocketAddress = new InetSocketAddress(this.port);
        } else {
            inetSocketAddress = new InetSocketAddress(this.address, this.port);
        }
        return inetSocketAddress ;
    }
}
