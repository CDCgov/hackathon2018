package com.javainuse.service;

import java.util.List;

import com.javainuse.model.Employee;

public interface EmployeeService {
	void insertEmployee(Employee emp);
	List<Employee> getAllEmployees();
	List<Employee> getSearch(String language);
}
