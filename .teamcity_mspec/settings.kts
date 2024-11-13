import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildSteps.DotnetMsBuildStep
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetMsBuild
import jetbrains.buildServer.configs.kotlin.buildSteps.mspec
import jetbrains.buildServer.configs.kotlin.buildSteps.nuGetInstaller
import jetbrains.buildServer.configs.kotlin.triggers.vcs
import jetbrains.buildServer.configs.kotlin.vcs.GitVcsRoot

/*
The settings script is an entry point for defining a TeamCity
project hierarchy. The script should contain a single call to the
project() function with a Project instance or an init function as
an argument.

VcsRoots, BuildTypes, Templates, and subprojects can be
registered inside the project using the vcsRoot(), buildType(),
template(), and subProject() methods respectively.

To debug settings scripts in command-line, run the

    mvnDebug org.jetbrains.teamcity:teamcity-configs-maven-plugin:generate

command and attach your debugger to the port 8000.

To debug in IntelliJ Idea, open the 'Maven Projects' tool window (View
-> Tool Windows -> Maven Projects), find the generate task node
(Plugins -> teamcity-configs -> teamcity-configs:generate), the
'Debug' option is available in the context menu for the task.
*/

version = "2024.03"

project {

    vcsRoot(HttpsGithubComBurkharttMspecTestGitRefsHeadsMaster)
    vcsRoot(HttpsGithubComAgrossMspecSamplesGitRefsHeadsMaster)
    vcsRoot(HttpsGithubComJonhiltMSpecExampleGitRefsHeadsMaster)

    buildType(MSpecTestProject)
    buildType(MSpecPrototype)
}

object MSpecPrototype : BuildType({
    name = "✅ MSpec Prototype"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        nuGetInstaller {
            id = "jb_nuget_installer"
            toolPath = "%teamcity.tool.NuGet.CommandLine.DEFAULT%"
            projects = "AlteraxPrototype.sln"
            updatePackages = updateParams {
            }
        }
        dotnetMsBuild {
            id = "dotnet"
            projects = "AlteraxPrototype.sln"
            version = DotnetMsBuildStep.MSBuildVersion.CrossPlatform
        }
        mspec {
            id = "mspec"
            mspecPath = """C:\Users\Administrator\.nuget\packages\machine.specifications.runner.console\1.0.0\tools"""
            includeTests = """AuthorMaintenenceResource.UnitTest\bin\Debug\netcoreapp2.0\AuthorMaintenenceResource.UnitTest.dll"""
        }
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
    }
})

object MSpecTestProject : BuildType({
    name = "MSpec Test Project"

    vcs {
        root(HttpsGithubComJonhiltMSpecExampleGitRefsHeadsMaster)
    }

    steps {
        dotnetMsBuild {
            id = "dotnet"
            projects = "MSpecExample.sln"
            version = DotnetMsBuildStep.MSBuildVersion.CrossPlatform
            args = "-restore -noLogo"
            sdk = "3.5"
        }
        mspec {
            id = "mspec"
            mspecPath = """C:\Users\Administrator\.nuget\packages\machine.specifications.runner.console\1.0.0\tools"""
            includeTests = "MSpecExample.Tests"
        }
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
    }
})

object HttpsGithubComAgrossMspecSamplesGitRefsHeadsMaster : GitVcsRoot({
    name = "https://github.com/agross/mspec-samples.git#refs/heads/master"
    url = "https://github.com/agross/mspec-samples.git"
    branch = "refs/heads/master"
    branchSpec = "refs/heads/*"
    authMethod = password {
        userName = ""
        password = ""
    }
})

object HttpsGithubComBurkharttMspecTestGitRefsHeadsMaster : GitVcsRoot({
    name = "https://github.com/DariaKrup/MSpecTest.git#refs/heads/master"
    url = "https://github.com/DariaKrup/MSpecTest.git"
    branch = "refs/heads/master"
    branchSpec = "refs/heads/*"
    authMethod = password {
        password = "credentialsJSON:3c3ff2ee-925a-4336-94d9-55fcb56575d9"
    }
})

object HttpsGithubComJonhiltMSpecExampleGitRefsHeadsMaster : GitVcsRoot({
    name = "https://github.com/jonhilt/MSpec-Example.git#refs/heads/master"
    url = "https://github.com/jonhilt/MSpec-Example.git"
    branch = "refs/heads/master"
    branchSpec = "refs/heads/*"
    authMethod = password {
        userName = ""
        password = ""
    }
})
