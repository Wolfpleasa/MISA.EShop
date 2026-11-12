pipeline {
    agent any

    stages {
        stage('Check .NET 5 Backend') {
            when {
                changeRequest target: 'main'
            }
            steps {
                dir('MISA.EShop.Backend') {
                    sh '''
                        dotnet --version
                        dotnet restore
                        dotnet build --no-restore --configuration Release
                    '''
                }
            }
        }

        stage('Check Vue 2 Frontend') {
            when {
                changeRequest target: 'main'
            }
            steps {
                dir('MISA.EShop.Frontend') {
                    sh '''
                        node --version
                        npm --version
                        npm ci
                        npm run build
                    '''
                }
            }
        }
    }
}
