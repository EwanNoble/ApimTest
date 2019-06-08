$null = [Reflection.Assembly]::LoadWithPartialName("System.Web")

try {
    $Location = "West Europe"
    $DeploymentParameters = @{
        ResourceGroupName       = "dev-vnet-integration-rg"
        TemplateFile            = ".\azure\template.json"
        TemplateParameterObject = @{
            sqlServerAdminPassword = [System.Web.Security.Membership]::GeneratePassword(10, 0).Replace("&", "1")
        }
    }

    New-AzureRmResourceGroup -Location $Location -Name $DeploymentParameters["ResourceGroupName"] -Force
    New-AzureRmResourceGroupDeployment @DeploymentParameters -Verbose:$VerbosePreference
}
catch {
    throw "Deployment failed: $_"
}



