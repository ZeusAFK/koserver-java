package koserver.login.database.entity;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Table;

import koserver.common.database.entity.AbstractDatabaseEntity;

@Entity
@Table(name="patch")
public class PatchEntity extends AbstractDatabaseEntity{

	private static final long serialVersionUID = 1L;

	@Column(name="version")
	private int version;
	
	@Column(name="filename")
	private String filename;
	
	public int getVersion(){
		return version;
	}
	
	public void setVersion(final int version){
		this.version = version;
	}
	
	public String getFileName(){
		return filename;
	}
	
	public void setFileName(String filename){
		this.filename = filename;
	}
}
