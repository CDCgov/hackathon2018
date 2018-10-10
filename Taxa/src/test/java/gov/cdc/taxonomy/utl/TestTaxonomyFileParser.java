package gov.cdc.taxonomy.utl;

import java.io.File;
import java.io.IOException;

import org.junit.Test;

import gov.cdc.taxonomy.util.TaxonomyFileParser;

public class TestTaxonomyFileParser {

	@Test
	public void testParser() throws IOException {
		String nodeId = "666";
		File file1 = new File ("C:\\Users\\czv8\\hackthon\\nodes.vibrio.dmp");
		File file2 = new File ("C:\\Users\\czv8\\hackthon\\names.vibrio.dmp");
		
		TaxonomyFileParser.parse(new File[] {file1,  file2}, nodeId);
		
	}
}
