package com.siran.tools;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.ArrayList;
import java.util.Date;

public class MigrationTool {

	ArrayList<String> allClasses;
	ArrayList<String> allOldClasses;
	ArrayList<String> allNewClasses;
	String tmpStamp;
	
	public static void main(String[] args) {
		if (args.length < 2) {
			System.out.println("Usage Axis1toAxis2 <Source> <schema classes> <Source output folder>");
			System.exit(0);
		}
		MigrationTool converter = new MigrationTool();
		converter.migratetoAxis2(args[0],args[1],args[2]);
	}

	private void migratetoAxis2(String srcDir, String classesDir, String srcOutDir) {
		Date dt = new Date();
		tmpStamp = Long.toString(dt.getTime());
		ArrayList<String> classNames = buildSchemaClassList(classesDir);
		for (String str: classNames) {
			System.out.println("ClassName: " + str);
		}
		updateSource(srcDir,srcOutDir);
	}

	private void updateSource(String srcDir, String srcOutDir) {
		File srcFile = new File(srcDir);
		if (srcFile.exists() && srcFile.isDirectory()) {
			updateSrcDir(srcFile,srcOutDir);
		}
		
	}

	private void updateSrcDir(File srcFile, String srcOutDir) {
		for (File file: srcFile.listFiles()) {
			if (file.isDirectory()) {
				updateSrcDir(file,srcOutDir);
			} else {
				updateSingleSource(file,srcOutDir);
			}
		}
		
	}

	private void updateSingleSource(File file, String srcOutDir) {
		try {			
			String fileName = file.getAbsolutePath();
			String backFileName = fileName.replaceAll(".java", "_java" + tmpStamp + ".bak");
			file.renameTo(new File(backFileName));
			BufferedReader infile = new BufferedReader(new InputStreamReader(new FileInputStream(backFileName)));
			BufferedWriter outfile = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(fileName)));
			String line1 = infile.readLine();
			while(line1 != null) {
				String updatedLine = updateLine(line1);
				outfile.write(updatedLine + System.lineSeparator());
			}
			infile.close();
			outfile.close();
		} catch(IOException ex) {
			ex.printStackTrace();
		} catch (Exception ex) {
			ex.printStackTrace();
		}
		
	}

	private String updateLine(String line1) {
		int i = 0;
		String nline = line1;
		while (nline.indexOf("  ") > 0)
			nline = nline.replace("  ", " ");
		for (String str: allOldClasses) {
			if (nline.indexOf(str) > 0) {
				String newClsName = allNewClasses.get(i);
				if (nline.indexOf("new " + str + "()") > 0) {
					line1 = line1.replace("new", "").replace(str + "()", newClsName + ".Factory.newInstance()");
				}
				line1 = line1.replace(str, newClsName);
			}
			i++;
		}
		return line1;
	}

	private ArrayList<String> buildSchemaClassList(String classesDir) {
		allClasses = new ArrayList<String>();
		allOldClasses = new ArrayList<String>();
		allNewClasses = new ArrayList<String>();
		File classFile = new File(classesDir);
		System.out.println(classFile.exists() + "," + classFile.isDirectory());
		if (classFile.exists() && classFile.isDirectory()) {
			addClasses(classFile);
		}
		return allClasses;
	}

	private void addClasses(File classesDir) {
		String packageName = getPackageName(classesDir);
		for (File file: classesDir.listFiles()) {
			
			if (file.isDirectory()) 
				addClasses(file);
			else {
				String clsName = file.getName().substring(0,file.getName().length() - 6); 				
				allClasses.add(packageName + "." + clsName);
				allClasses.add(clsName);
				if (clsName.endsWith("Type")) {
					String oldName = clsName.substring(0,clsName.length() - 4) + "_Type"; 					
					allOldClasses.add(packageName + "." + oldName);
					allOldClasses.add(oldName); 
					allNewClasses.add(packageName + "." + clsName);
					allNewClasses.add(clsName);
				} 
				if (clsName.endsWith("Enum")) {
					String oldName = clsName.substring(0,clsName.length() - 4) + "_Enum"; 					
					allOldClasses.add(packageName + "." + oldName);
					allOldClasses.add(oldName);
					allNewClasses.add(packageName + "." + clsName);
					allNewClasses.add(clsName);
				} 
				if (clsName.endsWith("Choice")) {
					String oldName = clsName.substring(0,clsName.length() - 6) + "_Choice"; 					
					allOldClasses.add(packageName + "." + oldName);
					allOldClasses.add(oldName);
					allNewClasses.add(packageName + "." + clsName);
					allNewClasses.add(clsName);
				} 
			}
		}		
	}

	private String getPackageName(File classesDir) {
		String name = classesDir.getAbsolutePath();
		String name1 = name.replace(File.separatorChar,'.');
		if (name1.indexOf(".com.") > 0) {
			name1 = name1.substring(name1.indexOf(".com.") + 1);
		} else if (name1.indexOf(".org.") > 0) {
			name1 = name1.substring(name1.indexOf(".org.") + 1);
		} 
		return name1;
	}
	
	
}
