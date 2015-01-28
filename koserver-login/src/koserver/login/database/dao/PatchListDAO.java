package koserver.login.database.dao;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import koserver.common.database.dao.AbstractDAO;
import koserver.login.database.entity.PatchEntity;
import koserver.login.services.DatabaseService;

import org.hibernate.Session;
import org.hibernate.Transaction;

public class PatchListDAO extends AbstractDAO<PatchEntity> {

	@Override
	public void create(final PatchEntity entity) {
		throw new RuntimeException("Can't create a patch !");
	}

	@Override
	public PatchEntity read(final Integer id) {
		PatchEntity patchList = null;

		final Transaction transaction = session.beginTransaction();

		patchList = (PatchEntity) session.get(PatchEntity.class, id);

		transaction.commit();

		return patchList;
	}

	public List<PatchEntity> readAll() {
		final List<PatchEntity> patchsEntities = new ArrayList<PatchEntity>();

		session.beginTransaction();
		@SuppressWarnings("unchecked")
		final Collection<? extends PatchEntity> patchs = session.createQuery("FROM PatchEntity").list();
		if (patchs != null) {
			patchsEntities.addAll(patchsEntities);
		}
		session.clear();
		session.getTransaction().commit();

		return patchsEntities;
	}

	@Override
	public void update(final PatchEntity entity) {
		throw new RuntimeException("Can't update a patch !");
	}

	@Override
	public void delete(final PatchEntity entity) {
		throw new RuntimeException("Can't delete a patch !");
	}

	@Override
	protected Session getSession() {
		return DatabaseService.getInstance().getSessionFactory().openSession();
	}
}
