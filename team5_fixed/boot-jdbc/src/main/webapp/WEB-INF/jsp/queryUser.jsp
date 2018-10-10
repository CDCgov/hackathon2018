<%@taglib uri="http://www.springframework.org/tags/form" prefix="form"%>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
<title>Search User</title>
</head>
<jsp:include page="menu.jsp" />
<body>
	<h3 style="color: red;">Search User</h3>

	<div id="addEmployee">
		<form:form action="/queryUser" method="post"
			modelAttribute="emp">			
			<p>
				<label>Language</label>
				<form:input path="languages" />
			</p>
			<input type="SUBMIT" value="Submit" />
		</form:form>
	</div>
</body>
</html>
