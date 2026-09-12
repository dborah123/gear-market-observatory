using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace GearMarketObservatory.Cdk;

public static class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        _ = new GearMarketObservatoryDevStack(app, "GearMarketObservatoryDev");
        app.Synth();
    }
}

/// <summary>
/// Durable AWS foundation for the single Phase 1 development environment.
/// </summary>
public sealed class GearMarketObservatoryDevStack : Stack
{
    private const string ProjectTag = "gear-market-observatory";
    private const string EnvironmentTag = "dev";
    private const string ManagedByTag = "cdk";

    public GearMarketObservatoryDevStack(Construct scope, string id)
        : base(scope, id, new StackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = Aws.ACCOUNT_ID,
                Region = "us-east-1"
            },
            Description = "Gear Market Observatory Phase 1 development foundation"
        })
    {
        Amazon.CDK.Tags.Of(this).Add("project", ProjectTag);
        Amazon.CDK.Tags.Of(this).Add("environment", EnvironmentTag);
        Amazon.CDK.Tags.Of(this).Add("managed-by", ManagedByTag);

        var rawArchive = new Bucket(this, "RawArchiveBucket", new BucketProps
        {
            BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
            Encryption = BucketEncryption.S3_MANAGED,
            Versioned = true,
            RemovalPolicy = RemovalPolicy.RETAIN,
            AutoDeleteObjects = false,
            LifecycleRules =
            [
                new LifecycleRule
                {
                    Id = "ExpireDevelopmentArchive",
                    Enabled = true,
                    Expiration = Duration.Days(365),
                    NoncurrentVersionExpiration = Duration.Days(30)
                }
            ]
        });

        var collectorLogGroup = new LogGroup(this, "CollectorLogGroup", new LogGroupProps
        {
            LogGroupName = "/aws/lambda/gear-market-observatory-dev-ebay-collector",
            Retention = RetentionDays.ONE_MONTH,
            RemovalPolicy = RemovalPolicy.RETAIN
        });

        var collectorRole = new Role(this, "CollectorExecutionRole", new RoleProps
        {
            RoleName = "gear-market-observatory-dev-collector",
            AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
            Description = "Least-privilege execution role for the Phase 1 collector"
        });

        collectorRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = ["logs:CreateLogStream", "logs:PutLogEvents"],
            Resources = [$"{collectorLogGroup.LogGroupArn}:*"]
        }));
        rawArchive.GrantWrite(collectorRole);

        _ = new CfnOutput(this, "RawArchiveBucketName", new CfnOutputProps
        {
            Description = "Private S3 bucket for raw source observations",
            Value = rawArchive.BucketName,
            ExportName = "GearMarketObservatoryDev-RawArchiveBucketName"
        });
        _ = new CfnOutput(this, "RawArchiveBucketArn", new CfnOutputProps
        {
            Description = "ARN of the raw source archive bucket",
            Value = rawArchive.BucketArn,
            ExportName = "GearMarketObservatoryDev-RawArchiveBucketArn"
        });
        _ = new CfnOutput(this, "CollectorExecutionRoleArn", new CfnOutputProps
        {
            Description = "Lambda execution role for the future collector",
            Value = collectorRole.RoleArn
        });
        _ = new CfnOutput(this, "CollectorLogGroupName", new CfnOutputProps
        {
            Description = "CloudWatch log group reserved for the future collector",
            Value = collectorLogGroup.LogGroupName
        });
        _ = new CfnOutput(this, "StackName", new CfnOutputProps
        {
            Description = "Deployed foundation stack name",
            Value = Aws.STACK_NAME
        });
    }
}
