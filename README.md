# Gear Market Observatory

Gear Market Observatory is a public monorepo for a small, source-first gear-market ingestion
platform. Phase 1 starts with a fixture-safe eBay collector, durable raw observations, and a
versioned event contract. AWS infrastructure is provisioned through CDK; live API access and
credentials remain out of this repository.

## Local prerequisites

- .NET SDK 10.0.401 or a compatible later patch release
- Node.js LTS and npm (needed for the AWS CDK CLI)
- Git

No AWS credentials or eBay credentials are required for the local build and tests.

## Build and test

From the repository root:

```powershell
dotnet test GearMarketObservatory.sln
dotnet build GearMarketObservatory.sln
```

The pull-request workflow also builds the solution and runs `cdk synth --lookups false`. It performs
no AWS deployment and requests no AWS credentials.

To synthesize locally after installing the CDK CLI:

```powershell
npm install --global aws-cdk
cd infrastructure/cdk
cdk synth --lookups false
```

To review and deploy the development foundation after CDK has been bootstrapped in `us-east-1`:

```powershell
cdk diff --lookups false
cdk deploy GearMarketObservatoryDev --require-approval never
```

The stack creates the private, versioned raw archive bucket and retains it during stack removal.
Its lifecycle expires current objects after 365 days and noncurrent versions after 30 days. The
bucket name, ARN, collector role ARN, log group name, and stack name are available as stack outputs.

## Repository layout

| Path | Purpose |
| --- | --- |
| `services/ebay-collector` | Lambda collector placeholder; fixture-backed behavior arrives in PR4 |
| `libraries/event-contracts` | Source-agnostic `ListingObserved` event contract |
| `infrastructure/cdk` | C# CDK application for the dev foundation and future application resources |
| `config` | Reviewed, non-secret source configuration |
| `tests` | Unit and compatibility tests |
| `test-fixtures` | Recorded or synthetic source payloads |

## Phase 1 boundaries

There is one `dev` AWS environment in `us-east-1`. The intended deployment path is CDK plus
GitHub Actions OIDC. PR2 provisions the application raw S3 archive, collector IAM role, and
CloudWatch log group. It does not deploy Lambda code, Kinesis, secrets, or a schedule.
