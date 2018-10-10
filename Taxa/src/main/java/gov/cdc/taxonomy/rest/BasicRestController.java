package gov.cdc.taxonomy.rest;

import java.io.BufferedOutputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.apache.tomcat.util.buf.StringUtils;
import org.apache.tomcat.util.http.fileupload.IOUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.multipart.MultipartFile;

import gov.cdc.taxonomy.model.TaxonomyNode;
import gov.cdc.taxonomy.util.TaxonomyFileParser;

@Controller
@RequestMapping("/")
public class BasicRestController {

	@Autowired
	RestTemplate restTemplate;
	@RequestMapping("/")
	  public String home() {
	    return "index.html";
	  }
	@RequestMapping("/add")
	  public String add() {
	    return "add.html";
	  }
	@RequestMapping(value = "/query", method = RequestMethod.POST,produces = { "application/json" })
	
	  @ResponseBody
	  public TaxonomyNode selectTaxonomy(@RequestParam("nodeFile") MultipartFile nodeFile,@RequestParam("nameFile") MultipartFile nameFile, @RequestParam("term")String term) {
		TaxonomyNode node = null;
		if (!nodeFile.isEmpty() && !nameFile.isEmpty()) {
            try {
               /* byte[] bytes = nodeFile.getBytes();
                BufferedOutputStream stream = 
                        new BufferedOutputStream(new FileOutputStream(new File(nodeFile.getName() + "-uploaded")));
                stream.write(bytes);
                stream.close();*/
            	File [] files = new File[] {multipartToFile(nodeFile), multipartToFile(nameFile)};
            	node = TaxonomyFileParser.parse(files, term);
             
            } catch (Exception e) {
               e.printStackTrace();
            }
        } 
		 return  node;
	  }
	@RequestMapping(value = "/create", method = RequestMethod.POST)
	public ResponseEntity<?> generateZip(@RequestParam("nodeFile") MultipartFile nodeFile,@RequestParam("nameFile") MultipartFile nameFile, @RequestParam("term")String term,  @RequestParam("rank")String rank,  @RequestParam("label")String label) throws IOException {
		 HttpHeaders headers = new HttpHeaders();
	        headers.setContentType(MediaType.parseMediaType("application/zip"));
	        String outputFilename = "output.zip";
	        headers.setContentDispositionFormData(outputFilename, outputFilename);
	        headers.setCacheControl("must-revalidate, post-check=0, pre-check=0");

	        ByteArrayOutputStream byteOutputStream = new ByteArrayOutputStream();
	        ZipOutputStream zipOutputStream = new ZipOutputStream(byteOutputStream);
	        File [] files = new File[] {multipartToFile(nodeFile), multipartToFile(nameFile)};
        	TaxonomyFileParser.addNode(files, term, rank, label);

	        for(File file: files) {
	            zipOutputStream.putNextEntry(new ZipEntry(file.getName()));           
	            FileInputStream fileInputStream = new FileInputStream(file);
	            IOUtils.copy(fileInputStream, zipOutputStream);
	            fileInputStream.close();
	            zipOutputStream.closeEntry();
	        }           
	        zipOutputStream.close();
	    return new ResponseEntity<>(byteOutputStream.toByteArray(), headers, HttpStatus.OK); 
	}
	
	public File multipartToFile(MultipartFile multipart) throws IllegalStateException, IOException 
	{
	    String name = multipart.getOriginalFilename().substring(multipart.getOriginalFilename().lastIndexOf("\\")+1);
	    File newFile = new File("data/"+name);
	    byte[] bytes = multipart.getBytes();
        BufferedOutputStream stream = 
                new BufferedOutputStream(new FileOutputStream(newFile));
        stream.write(bytes);
        stream.close();
	 /*  File newFile = new File("data/"+ convFile.getName());
	   if(newFile.exists()) {
		   newFile.delete();
	   }
	    Files.copy(Paths.get(convFile.getAbsolutePath()), Paths.get(newFile.getAbsolutePath()));*/
	    return newFile;
	}

}
