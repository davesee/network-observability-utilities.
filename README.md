# NetworkObservabilityUtilities

This repo contains the common nuget packages necessary for running any of the Network Observability application seeds.

## Common Project

Contains basic common utility logic.

## DataModel

Contains all the logic for interacting with the database through EF6 models.

## Observability

Contains classes for metric collection and observability.

## RabbitMQ

Contains classes for interacting with rabbitmq, along with the message classes.

## Publishing The Packages

When the template repository is initially copied or changes are made to the code, a pipeline will run to publish the packages to the current Github project registry.

![gitlab registry](./docs/images/githubPackages.png)

**Note:** the <RepositoryUrl> tag in each project, will need to be updated with the correct value.

![project file](./docs/images/repoUrl.png)

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
