package gov.cdc.taxonomy.model;

public class Authority {
	
	private String taxId;
	private String nameTxt;
	private String uniqueName;
	private String nameClass;
	
	public Authority (String str) {
		String[] arr = str.split("\\|");
		this.taxId = arr[0].trim();
		this.nameTxt = arr[1].trim();
		this.uniqueName = arr[2].trim();
		this.nameClass = arr[3].trim();
	}

	public String getTaxId() {
		return taxId;
	}

	public void setTaxId(String taxId) {
		this.taxId = taxId;
	}

	public String getNameTxt() {
		return nameTxt;
	}

	public void setNameTxt(String nameTxt) {
		this.nameTxt = nameTxt;
	}

	public String getUniqueName() {
		return uniqueName;
	}

	public void setUniqueName(String uniqueName) {
		this.uniqueName = uniqueName;
	}

	public String getNameClass() {
		return nameClass;
	}

	public void setNameClass(String nameClass) {
		this.nameClass = nameClass;
	}

}
