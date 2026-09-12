# Gear Market Observatory

Gear Market Observatory is a public monorepo for a small, source-first gear-market ingestion
platform. Phase 1 starts with a fixture-safe eBay collector, durable raw observations, and a
versioned event contract. AWS infrastructure, live API access, and credentials are intentionally
not part of this repository baseline; they arrive in later, independently reviewable PRs.

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

The pull-request workflow also builds the solution and runs `cdk synth --lookups false` against the
empty CDK baseline. It performs no AWS deployment and requests no AWS credentials.

To synthesize locally after installing the CDK CLI:

```powershell
npm install --global aws-cdk
cd infrastructure/cdk
cdk synth --lookups false
```

## Repository layout

| Path | Purpose |
| --- | --- |
| `services/ebay-collector` | Lambda collector placeholder; fixture-backed behavior arrives in PR4 |
| `libraries/event-contracts` | Source-agnostic `ListingObserved` event contract |
| `infrastructure/cdk` | C# CDK application placeholder; AWS resources arrive in PR2 |
| `config` | Reviewed, non-secret source configuration |
| `tests` | Unit and compatibility tests |
| `test-fixtures` | Recorded or synthetic source payloads |

## Phase 1 boundaries

There is one `dev` AWS environment in `us-east-1`. The intended deployment path is CDK plus
GitHub Actions OIDC, but this baseline has no deployment workflow, AWS credentials, live eBay
calls, application S3 bucket, Kinesis stream, secrets, Lambda deployment, or schedule.
