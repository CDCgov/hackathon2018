package com.javainuse.model;

public class Employee {

	private String empId;
	private String empName;
	private String agency;
	private String office;
	private String languages;

	public String getEmpId() {
		return empId;
	}

	public void setEmpId(String empId) {
		this.empId = empId;
	}

	public String getEmpName() {
		return empName;
	}

	public void setEmpName(String empName) {
		this.empName = empName;
	}

	public String getAgency() {
		return agency;
	}

	public void setAgency(String agency) {
		this.agency = agency;
	}

	public String getOffice() {
		return office;
	}

	public void setOffice(String office) {
		this.office = office;
	}

	public String getLanguages() {
		return languages;
	}

	public void setLanguages(String languages) {
		this.languages = languages;
	}

	@Override
	public String toString() {
		return "User [uid=" + empId + ", User Name=" + empName + ", Agency=" + agency + ", Office=" + office + ", Langauage=" + languages + "]";
	}

}

