using Microsoft.Owin;
using Owin;
using Microsoft.Azure.KeyVault;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(azure_keyvault_exercise.App_Start.Startup))]

namespace azure_keyvault_exercise.App_Start
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(EncryptionHelper.GetToken));

			Task<Microsoft.Azure.KeyVault.Models.SecretBundle> task = kv.GetSecretAsync(WebConfigurationManager.AppSettings["SecretUri"]);

			task.ContinueWith(continuation =>
			{
				Microsoft.Azure.KeyVault.Models.SecretBundle secretBundle = continuation.Result;
				EncryptionHelper.EncryptSecret = secretBundle.Value;
			});
		}
	}
}