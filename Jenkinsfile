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
    post {
        failure {
            script {
                if (env.CHANGE_ID && env.CHANGE_TARGET == 'main' && env.CHANGE_URL) {
                    try {
                        timeout(time: 5, unit: 'MINUTES') {
                            sh '''
                                set +x
                                OPENSRE_NO_TELEMETRY=1 opensre ask --dangerously-bypass-approvals \
                                  "Analyze GitHub PR ${CHANGE_URL} using failed Jenkins build ${BUILD_URL}.
                                   Read the PR diff and build log. Identify the root cause
                                   with evidence and suggest a concrete fix in Vietnamese.
                                   Check existing PR comments first. If no comment already
                                   covers this failure, post your analysis on this PR.
                                   Do not modify code, push, or deploy.
                                   State clearly whether you posted a comment."
                            '''
                        }
                    } catch (err) {
                        echo "OpenSRE không hoàn tất: ${err}"
                    }
                }
            }
        }
    }
}
