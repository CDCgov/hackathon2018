package com.javainuse.dao.impl;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import javax.annotation.PostConstruct;
import javax.sql.DataSource;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.BatchPreparedStatementSetter;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.jdbc.core.support.JdbcDaoSupport;
import org.springframework.stereotype.Repository;

import com.javainuse.dao.EmployeeDao;
import com.javainuse.model.Employee;

@Repository
public class EmployeeDaoImpl extends JdbcDaoSupport implements EmployeeDao{
	
	@Autowired 
	DataSource dataSource;
	
	@PostConstruct
	private void initialize(){
		setDataSource(dataSource);
	}
	
	@Override
	public void insertEmployee(Employee emp) {
		String sql = "INSERT INTO employee " +
				"(empId, empName, agency, office, languages) VALUES (?, ?, ?, ?, ?)" ;
		getJdbcTemplate().update(sql, new Object[]{
				emp.getEmpId(), emp.getEmpName(), emp.getAgency(), emp.getOffice(),emp.getLanguages()
		});
	}
	
	@Override
	public List<Employee> getAllEmployees(){
		String sql = "SELECT * FROM employee";
		List<Map<String, Object>> rows = getJdbcTemplate().queryForList(sql);
		
		List<Employee> result = new ArrayList<Employee>();
		for(Map<String, Object> row:rows){
			Employee emp = new Employee();
			emp.setEmpId((String)row.get("empId"));
			emp.setEmpName((String)row.get("empName"));
			emp.setAgency((String)row.get("agency"));
			emp.setOffice((String)row.get("office"));
			emp.setLanguages((String)row.get("languages"));
			result.add(emp);
		}
		
		return result;
	}
	
	@Override
	public List<Employee> getSearch(String language){
		String sql = "SELECT * FROM employee where languages = '" + language +"'";
		List<Map<String, Object>> rows = getJdbcTemplate().queryForList(sql);
		
		List<Employee> result = new ArrayList<Employee>();
		for(Map<String, Object> row:rows){
			Employee emp = new Employee();
			emp.setEmpId((String)row.get("empId"));
			emp.setEmpName((String)row.get("empName"));
			emp.setAgency((String)row.get("agency"));
			emp.setOffice((String)row.get("office"));
			emp.setLanguages((String)row.get("languages"));
			result.add(emp);
		}
		
		return result;
	}

}
