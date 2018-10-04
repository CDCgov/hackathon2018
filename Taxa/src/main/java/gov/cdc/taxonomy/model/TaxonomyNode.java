package gov.cdc.taxonomy.model;

import java.io.Serializable;
import java.util.List;

public class TaxonomyNode implements Serializable {
	
	private List<ChildTaxonomyNode> children;
	
	private List<Authority> authorities;
	private String taxId;
	private String parentId;
	private String rank;
	
	public TaxonomyNode(String strNode) {
		String[] arr = strNode.split("\\|");
		this.taxId = arr[0].trim();
		this.parentId = arr[1].trim();
		this.rank = arr[2].trim();
	}
	
	public List<Authority> getAuthorities() {
		return authorities;
	}

	public void setAuthorities(List<Authority> authorities) {
		this.authorities = authorities;
	}

	public String getTaxId() {
		return taxId;
	}

	public void setTaxId(String taxId) {
		this.taxId = taxId;
	}

	public String getParentId() {
		return parentId;
	}

	public void setParentId(String parentId) {
		this.parentId = parentId;
	}

	public String getRank() {
		return rank;
	}

	public void setRank(String rank) {
		this.rank = rank;
	}

	public List<ChildTaxonomyNode> getChildren() {
		return children;
	}

}
