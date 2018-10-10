<%@taglib uri="http://www.springframework.org/tags/form" prefix="form"%>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
<title>Add User</title>
</head>
<jsp:include page="menu.jsp" />
<body>
	<h3 style="color: red;">Add New User</h3>

	<div id="addEmployee">
		<form:form action="/addNewEmployee" method="post"
			modelAttribute="emp">
			<p>
				<label>Enter User Id</label>
				<form:input path="empId" />
			</p>
			<p>
				<label>Enter User Name</label>
				<form:input path="empName" />
			</p>
			<p>
				<label>Agency</label>
				<form:input path="agency" />
			</p>
			<p>
				<label>Office</label>
				<form:input path="office" />
			</p>
			<p>
				<label>Language</label>
				<form:input path="languages" />
			</p>
			<input type="SUBMIT" value="Submit" />
		</form:form>
	</div>
</body>
</html>
