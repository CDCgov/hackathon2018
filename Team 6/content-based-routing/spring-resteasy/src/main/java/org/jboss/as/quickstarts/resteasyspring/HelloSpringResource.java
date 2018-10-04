/*
 * JBoss, Home of Professional Open Source
 * Copyright 2015, Red Hat, Inc. and/or its affiliates, and individual
 * contributors by the @authors tag. See the copyright.txt in the
 * distribution for a full listing of individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.jboss.as.quickstarts.resteasyspring;

import javax.net.ssl.HttpsURLConnection;
import javax.ws.rs.Consumes;
import javax.ws.rs.core.Response;
import javax.ws.rs.GET;
import javax.ws.rs.MatrixParam;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.PUT;
import javax.ws.rs.QueryParam;

import org.codehaus.jackson.map.ObjectMapper;


import org.json.JSONArray;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;


/**
 *
 * @author Joshua Wilson
 *
 */
@Path("/")
public class HelloSpringResource {

    @Autowired
    GreetingBean greetingBean;

    /**
     * Create a default REST endpoint that directs the user to use the demonstration endpoints.
     *
     * @return html
     */
    @GET
    @Produces("text/html")
    public Response getDefault() {
        String msg = "Hello. <br> Please try <a href='http://localhost:8080/jboss-spring-resteasy/hello?name=yourname'>jboss-spring-resteasy/hello?name=yourname</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/basic'>jboss-spring-resteasy/basic</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/queryParam?param=query'>jboss-spring-resteasy/queryParam?param=query</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/matrixParam;param=matrix'>jboss-spring-resteasy/matrixParam;param=matrix</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/uriParam/789'>jboss-spring-resteasy/uriParam/789</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/locating/hello?name=yourname'>jboss-spring-resteasy/locating/hello?name=yourname</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/locating/basic'>jboss-spring-resteasy/locating/basic</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/locating/queryParam?param=query'>jboss-spring-resteasy/locating/queryParam?param=query</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/locating/matrixParam;param=matrix'>jboss-spring-resteasy/locating/matrixParam;param=matrix</a>"
            + "<br> Or try <a href='http://localhost:8080/jboss-spring-resteasy/locating/uriParam/789'>jboss-spring-resteasy/locating/uriParam/789</a>";
        System.out.println("getDefault()");
        return Response.ok(msg).build();
    }

    @GET
    @Path("hello")
    @Produces("text/plain")
    public Response sayHello(@QueryParam("name") String name) {
        String greetingMsg = greetingBean.greet("Hi Sam");
        System.out.println("Sending greeing: " + greetingMsg);
        return Response.ok(greetingMsg).build();
    }
    
    
    @GET
    @Path("fhir")
    @Produces("text/plain")
    public Response fhir() {
        ObjectMapper mapper = new ObjectMapper();

        String url = "http://object.datalake.REDACTED/api/1.0/deathonfhir/team6/find?from=0&order=1&size=-1";
        String returnVal = "rest call failed";
        try {
            URL obj = new URL(url);
            HttpURLConnection con = (HttpURLConnection) obj.openConnection();
            con.setRequestMethod("POST");
            con.setRequestProperty("Content-Type", "application/json");
            con.setDoOutput(true);
            String body = "{}";
            con.getOutputStream().write(body.getBytes("UTF8"));

            int responseCode = con.getResponseCode();

            BufferedReader in = new BufferedReader(
                    new InputStreamReader(con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();
            JSONObject jsonObject = new JSONObject(response.toString());
            ArrayList<DeathData> list = new ArrayList<>();
            JSONArray jsonArray = jsonObject.getJSONArray("items");
            if (jsonArray != null) {
                int len = jsonArray.length();
                for (int i=0;i<len;i++){
                    DeathData deathData = mapper.readValue(jsonArray.get(i).toString(),DeathData.class);
                    list.add(deathData);

                    //NVDRS Check
                    if(deathData.getUcod() != null && !deathData.getUcod().isEmpty()){
                        if(
                                deathData.getUcod().equalsIgnoreCase("X70") ||
                                deathData.getUcod().equalsIgnoreCase("X72") ||
                                deathData.getUcod().equalsIgnoreCase("X74") ||
                                deathData.getUcod().equalsIgnoreCase("X93") ||
                                deathData.getUcod().equalsIgnoreCase("X94") ||
                                deathData.getUcod().equalsIgnoreCase("X95") ||
                                deathData.getUcod().equalsIgnoreCase("Y06") ||
                                deathData.getUcod().equalsIgnoreCase("Y07") ||
                                deathData.getUcod().equalsIgnoreCase("W32") ||
                                deathData.getUcod().equalsIgnoreCase("W33") ||
                                deathData.getUcod().equalsIgnoreCase("W34")
                           ){
                            //send to NVDRS
                            sendToNVDRS(jsonArray.get(i).toString());
                        }
                    }

                    //SEND TO BIOSENSE
                    sendToBiosense(jsonArray.get(i).toString());
                    System.out.println("SENDING TO BIOSENSE: " + i);
                }
            }
            returnVal = "Data Sent";
        }catch (Exception e){
            returnVal = e.getMessage();
        }



        return Response.ok(returnVal).build();
    }


    private void sendToNVDRS(String jsonVal){
        String url2 = " http://nvdrs-restful-ha-team6.REDACTED/jboss-spring-resteasy/deathrecord";
        try {
            URL obj = new URL(url2);
            HttpURLConnection con = (HttpURLConnection) obj.openConnection();
            con.setRequestMethod("PUT");
            con.setRequestProperty("Content-Type", "application/json");
            con.setDoOutput(true);
            String body = jsonVal;
            con.getOutputStream().write(body.getBytes("UTF8"));

            BufferedReader in = new BufferedReader(
                    new InputStreamReader(con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();
        }catch (Exception e){
            e.printStackTrace();
        }
    }

    private void sendToBiosense(String jsonVal){
        String url2 = "http://biosense-restful-ha-team6.REDACTED/jboss-spring-resteasy/deathrecord";

        try {
            URL obj = new URL(url2);
            HttpURLConnection con = (HttpURLConnection) obj.openConnection();
            con.setRequestMethod("PUT");
            con.setRequestProperty("Content-Type", "application/json");
            con.setDoOutput(true);
            String body = jsonVal;
            con.getOutputStream().write(body.getBytes("UTF8"));

            BufferedReader in = new BufferedReader(
                    new InputStreamReader(con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();
        }catch (Exception e){
            e.printStackTrace();
        }
    }

    @GET
    @Path("basic")
    @Produces("text/plain")
    public String getBasic() {
        System.out.println("getBasic()");
        return "basic";
    }

    @PUT
    @Path("basic")
    @Consumes("text/plain")
    public void putBasic(String body) {
        System.out.println(body);
    }

    @GET
    @Path("queryParam")
    @Produces("text/plain")
    public String getQueryParam(@QueryParam("param") String param) {
        return param;
    }

    @GET
    @Path("matrixParam")
    @Produces("text/plain")
    public String getMatrixParam(@MatrixParam("param") String param) {
        return param;
    }

    @GET
    @Path("uriParam/{param}")
    @Produces("text/plain")
    public int getUriParam(@PathParam("param") int param) {
        return param;
    }

}
