
package gov.cdc.taxonomy.util;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import gov.cdc.taxonomy.model.Authority;
import gov.cdc.taxonomy.model.ChildTaxonomyNode;
import gov.cdc.taxonomy.model.TaxonomyNode;

public class TaxonomyFileParser {

	/**
	 * Parse the taxonomy file into
	 */
	public static TaxonomyNode parse(File[] files, String nodeId) throws IOException {
		Optional<String> node = Files.newBufferedReader(Paths.get(files[0].getAbsolutePath())).lines()
				.filter(s -> s.trim().startsWith(nodeId)).findFirst();
		if (node.isPresent()) {
			TaxonomyNode nd = new TaxonomyNode(node.get());
			// now parse the second file for descendents
			final List<Authority> authorities = new ArrayList<>();
			Files.newBufferedReader(Paths.get(files[1].getAbsolutePath())).lines()
					.filter(s -> s.trim().startsWith(nodeId)).forEach(s -> {
						authorities.add(new Authority(s));
					});
			if (!authorities.isEmpty()) {
				nd.setAuthorities(authorities);
			}
			return nd;
		}
		return null;
	}

	/**
	 * add a new entry to node file, and two entries to name file
	 * 
	 * @param files
	 *            array of two files, node file and name file
	 * @param parentId
	 * @param rank
	 * @param label
	 * @throws IOException
	 */
	public static void addNode(File[] files, String parentId, String rank, String label) throws IOException {

		final List<Integer> listNdId = new ArrayList<>();
		listNdId.add(0);

		Files.newBufferedReader(Paths.get(files[0].getAbsolutePath())).lines().forEach(s -> {
			int ndId = listNdId.get(0);
			String[] arr = s.split("\\|");
			if (arr.length > 1) {
				int pId = new Integer(arr[1].trim());
				int taxId = new Integer(arr[0].trim());
				if (pId > ndId) {
					ndId = pId;
				}
				if (taxId > ndId) {
					ndId = taxId;
				}
				listNdId.add(0, ndId);
			}
		});

		String strNode = createNodeEntry(listNdId.get(0) + 1, parentId, rank);
		String strName = createNameEntry(listNdId.get(0) + 1, label);

		Files.write(Paths.get(files[0].getAbsolutePath()), strNode.getBytes(), StandardOpenOption.APPEND);

		Files.write(Paths.get(files[1].getAbsolutePath()), strName.getBytes(), StandardOpenOption.APPEND);

	}

	private static String createNameEntry(Integer ndId, String label) {
		return ndId + "|" + label + "||scientific name|\r\n" + ndId + " | user supplied | | authority|\r\n";

	}

	private static String createNodeEntry(Integer ndId, String parentId, String rank) {
		return ndId + "|" + parentId + "|" + rank + "|||||||||||\r\n";
	}

}
