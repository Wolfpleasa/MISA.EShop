pipeline {
    agent any

    stages {
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
