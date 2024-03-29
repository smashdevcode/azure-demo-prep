﻿using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AzureDemoPrep.Shared.Storage
{
	public class BlobStorage
	{
		private const string DEFAULT_CONNECTION_STRING_NAME = "StorageConnectionString";

		public static CloudBlobClient GetClient()
		{
			// get a reference to the storage account
			var storageAccount = CloudStorageAccount.Parse(
				CloudConfigurationManager.GetSetting(DEFAULT_CONNECTION_STRING_NAME));

			// return the blob client
			return storageAccount.CreateCloudBlobClient();
		}
		public static CloudBlobContainer GetContainer(string containerName)
		{
			var client = GetClient();

			// get a reference to the container
			var container = client.GetContainerReference(containerName);

			// create the container if it doesn't exist
			container.CreateIfNotExist();

			// set the permissions on the container so that blobs are visible to the public
			container.SetPermissions(new BlobContainerPermissions()
			{
				PublicAccess = BlobContainerPublicAccessType.Blob
			});

			return container;
		}
		public static CloudBlob GetBlob(string containerName, string fileName)
		{
			return GetContainer(containerName).GetBlobReference(fileName);
		}
		public static CloudBlob GetNewBlob(string containerName, string fileName, out string newFileName)
		{
			var fileExtension = Path.GetExtension(fileName);
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			newFileName = string.Format("{0} {1:yyyyMMddhhmmss}{2}",
				fileNameWithoutExtension, DateTime.Now, fileExtension);
			return GetBlob(containerName, newFileName);
		}
		public static void DeleteBlob(string containerName, string fileName)
		{
			var blob = GetBlob(containerName, fileName);
			try
			{
				blob.Delete();
			}
			catch (StorageClientException exc)
			{
				Trace.WriteLine(exc.Message);
			}
		}
		public static string UploadBlob(string containerName, string fileName, Stream fileData)
		{
			// retrieve reference to the blob
			string newFileName = null;
			var blob = BlobStorage.GetNewBlob(containerName, fileName, out newFileName);

			// create the blob
			blob.UploadFromStream(fileData);

			// return the new file name
			return newFileName;
		}
	}
}
