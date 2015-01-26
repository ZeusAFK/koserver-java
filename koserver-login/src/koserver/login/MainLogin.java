package koserver.login;

import koserver.common.utils.PrintUtils;
import koserver.login.configuration.LoginConfig;
import koserver.login.services.ConfigurationService;
import koserver.login.services.DatabaseService;
import koserver.login.services.NetworkService;

public class MainLogin {

	public static void main(String[] args) {
		final long start = System.currentTimeMillis();

		PrintUtils.printSection("Services");
		ConfigurationService.getInstance().start();
		DatabaseService.getInstance().start();
		NetworkService.getInstance().start();
		PrintUtils.printSection("Launching login server version: " + LoginConfig.LOGIN_VERSION);

		System.out.println("Server started in " + ((System.currentTimeMillis() - start) / 1000) + " seconds");
	}
}