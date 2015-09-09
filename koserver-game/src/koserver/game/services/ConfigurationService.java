package koserver.game.services;

import java.util.Properties;

import koserver.common.config.ConfigurableProcessor;
import koserver.common.services.AbstractService;
import koserver.common.utils.PropertiesUtils;
import koserver.game.configuration.GameConfig;
import koserver.game.configuration.GlobalConfig;
import koserver.game.configuration.NetworkConfig;
import koserver.game.configuration.PlayerConfig;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;

public class ConfigurationService extends AbstractService {

	private static Logger log = Logger.getLogger(ConfigurationService.class.getName());

	private ConfigurationService() {
	}

	@Override
	public void onInit() {
		PropertyConfigurator.configure(System.getProperty("user.dir") + System.getProperty("file.separator") + "config" + System.getProperty("file.separator") + "log4j.properties");

		try {
			final Properties[] properties = PropertiesUtils.loadAllFromDirectory("config");
			ConfigurableProcessor.process(NetworkConfig.class, properties);
			ConfigurableProcessor.process(GameConfig.class, properties);
			ConfigurableProcessor.process(PlayerConfig.class, properties);
			ConfigurableProcessor.process(GlobalConfig.class, properties);
		} catch (final Exception e) {
			log.fatal("Can't load gameserver configurations", e);
			throw new Error("Can't load gameserver configurations", e);
		}
		log.info("ConfigurationService started");
	}

	@Override
	public void onDestroy() {
		log.info("ConfigurationService stopped");
	}

	public static ConfigurationService getInstance() {
		return SingletonHolder.instance;
	}

	private static class SingletonHolder {
		protected static final ConfigurationService instance = new ConfigurationService();
	}
}
