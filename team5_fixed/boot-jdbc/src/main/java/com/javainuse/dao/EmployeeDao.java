package com.javainuse.dao;

import java.util.List;

import com.javainuse.model.Employee;

public interface EmployeeDao {
	void insertEmployee(Employee cus);
	List<Employee> getAllEmployees();
	List<Employee> getSearch(String language);
}