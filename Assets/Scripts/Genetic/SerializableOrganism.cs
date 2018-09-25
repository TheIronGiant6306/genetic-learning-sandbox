using System;
using System.IO;
using System.Collections;
using AForge.Genetic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[Serializable]
public class SerializableOrganism {
	private double[] chromosomeValues;
	private double fitness;


	public SerializableOrganism(DoubleArrayChromosome chromosome) {
		chromosomeValues = chromosome.Value;
		fitness = chromosome.Fitness;
	}



	public void serialize(string path) {
		IFormatter formatter = new BinaryFormatter();

		if (!path.EndsWith(".chrom")) {
			path = path + ".chrom";
		}

		Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);  
		formatter.Serialize(stream, this);  
		stream.Close();  

	}

	public static void serialize(string path, SerializableOrganism chrom) {
		IFormatter formatter = new BinaryFormatter();

		if (!path.EndsWith(".chrom")) {
			path = path + ".chrom";
		}

		Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);  
		formatter.Serialize(stream, chrom);  
		stream.Close();  

	}

	public static void serialize(string path, DoubleArrayChromosome chrom) {
		IFormatter formatter = new BinaryFormatter();

		if (!path.EndsWith(".chrom")) {
			path = path + ".chrom";
		}

		SerializableOrganism serializableChrom = new SerializableOrganism (chrom);

		Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);  
		formatter.Serialize(stream, serializableChrom);  
		stream.Close();  

	}

	public static SerializableOrganism reconstitute(string path) {
		SerializableOrganism chrom = null;

		if (!path.EndsWith(".chrom")) {
			path = path + ".chrom";
		}

		FileStream fs = new FileStream(path, FileMode.Open);
		try 
		{
			BinaryFormatter formatter = new BinaryFormatter();

			chrom = (SerializableOrganism) formatter.Deserialize(fs);
		}
		catch (SerializationException e) 
		{
			Console.WriteLine("Failed to deserialize: " + e.Message);
			throw;
		}
		finally 
		{
			fs.Close();
		}

		return chrom;
	}




	public double[] getValues() {
		return chromosomeValues;
	}

	public double getFitness() {
		return fitness;
	}







	// : add support for network topologies




}
