package koserver.game.services;

import java.io.File;

import koserver.common.services.AbstractService;
import koserver.game.database.entity.AccountEntity;
import koserver.game.database.entity.PlayerEntity;

import org.apache.log4j.Logger;
import org.hibernate.SessionFactory;
import org.hibernate.cfg.AnnotationConfiguration;

@SuppressWarnings("deprecation")
public class DatabaseService extends AbstractService {

	private static Logger log = Logger.getLogger(DatabaseService.class.getName());

	private AnnotationConfiguration annotationConfiguration;
	private SessionFactory sessionFactory;

	private DatabaseService() {
		try {
			this.annotationConfiguration = new AnnotationConfiguration().configure(new File("config/hibernate.cfg.xml")).addPackage("koserver.game.database.entity");

			this.annotationConfiguration.addAnnotatedClass(PlayerEntity.class);
			this.annotationConfiguration.addAnnotatedClass(AccountEntity.class);

			this.sessionFactory = this.annotationConfiguration.buildSessionFactory();
		} catch (final Throwable ex) {
			System.err.println("Initial SessionFactory creation failed." + ex);
			throw new ExceptionInInitializerError(ex);
		}
	}

	@Override
	public void onInit() {
		log.info("DatabaseService started");
	}

	@Override
	public void onDestroy() {
	}

	public SessionFactory getSessionFactory() {
		return this.sessionFactory;
	}

	public static DatabaseService getInstance() {
		return SingletonHolder.instance;
	}

	private static class SingletonHolder {
		protected static final DatabaseService instance = new DatabaseService();
	}
}
