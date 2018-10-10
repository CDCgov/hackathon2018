package com.javainuse.controllers;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.validation.Errors;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.servlet.ModelAndView;

import com.javainuse.model.Employee;
import com.javainuse.service.EmployeeService;

@Controller
public class EmployeeController {

	@Autowired
	EmployeeService employeeService;

    //the welcome page
	@RequestMapping("/welcome")
	public ModelAndView firstPage() {
		return new ModelAndView("welcome");
	}

    //show the add employee form and also pass an empty backing bean object to store the form field values
	@RequestMapping(value = "/addNewEmployee", method = RequestMethod.GET)
	public ModelAndView show() {
		return new ModelAndView("addEmployee", "emp", new Employee());
	}

    //Get the form field vaues which are populated using the backing bean and store it in db
	@RequestMapping(value = "/addNewEmployee", method = RequestMethod.POST)
	public ModelAndView processRequest(@ModelAttribute("emp") Employee emp) {
		employeeService.insertEmployee(emp);
		List<Employee> employees = employeeService.getAllEmployees();
		ModelAndView model = new ModelAndView("getEmployees");
		model.addObject("employees", employees);
		return model;
	}

    //show all employees saved in db
	@RequestMapping("/getEmployees")
	public ModelAndView getEmployees() {
		List<Employee> employees = employeeService.getAllEmployees();
		ModelAndView model = new ModelAndView("getEmployees");
		model.addObject("employees", employees);
		return model;
	}
	
	//show the add employee form and also pass an empty backing bean object to store the form field values
		@RequestMapping(value = "/queryUser", method = RequestMethod.GET)
		public ModelAndView show1() {
			return new ModelAndView("queryUser", "emp", new Employee());
		}

	    //Get the form field vaues which are populated using the backing bean and store it in db
		@RequestMapping(value = "/queryUser", method = RequestMethod.POST)
		public ModelAndView processRequest1(@ModelAttribute("emp") Employee emp) {
			List<Employee> employees = employeeService.getSearch(emp.getLanguages());
			ModelAndView model = new ModelAndView("searchResult");
			model.addObject("employees", employees);
			return model;
		}

	
			
		

}
