'use strict';
const { success, failure } = require('./common/responses.js')
const AWS = require('aws-sdk')
AWS.config.region = 'eu-west-3';
var cognitoIdentityServiceProvider = new AWS.CognitoIdentityServiceProvider();

const ClientId = '7slrgq1k402900vbm819ivs773'

exports.handler = async (event) => {
  const data = event.hasOwnProperty('body') ? getData(decodeURIComponent(event.body)) : undefined

  try {
    switch (event.httpMethod) {
      case "GET":
        if (event.path.endsWith('username')) {
          if (!event.queryStringParameters) return failure({ message: 'Missing parameters' })
          return returnUsername(event.queryStringParameters.accessToken)
        }
        return failure({ message: 'Wrong GET url' })
      case "POST":
        if (event.path.endsWith('signin')) {
          return signin(data)
        } else if (event.path.endsWith('refreshaccesstoken')) {
          return refreshAccessToken(data)
        } else if (event.path.endsWith('signup')) {
          return signup(data)
        } else if (event.path.endsWith('edituser/password')) {
          return changePassword(data)
        } else if (event.path.endsWith('forgotpassword')) {
          if (event.queryStringParameters.confirm === 'true') {
            return confirmForgotPassword(data)
          } else {
            return forgotPassword(data)
          }
        }
        return failure({ message: 'Wrong POST url' })
      default:
        return success({ message: 'The request type is not defined' })
    }
  } catch (error) {
    console.error('Error', error)
    return failure(error)
  }
}

const getData = (body) => {
  let fields = body.split('&')
  let data = {}
  fields.forEach(field => {
    let values = field.split('=')
    data[values[0]] = values[1]
  })
  return data
}

const getUsername = async (accessToken) => {
  let userResponse = await cognitoIdentityServiceProvider.getUser({ AccessToken: accessToken }).promise()
  return userResponse.Username
}

const signin = async (data) => {
  let params = {
    AuthFlow: 'USER_PASSWORD_AUTH',
    ClientId: ClientId,
    AuthParameters: {
      'USERNAME': data.username,
      'PASSWORD': data.password
    }
  };
  try {
    let authResponse = await cognitoIdentityServiceProvider.initiateAuth(params).promise()

    let response = authResponse.AuthenticationResult
    response.Username = await getUsername(authResponse.AuthenticationResult.AccessToken)

    return success(response)
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const refreshAccessToken = async (data) => {
  let params = {
    AuthFlow: 'REFRESH_TOKEN_AUTH',
    ClientId: ClientId,
    AuthParameters: {
      'REFRESH_TOKEN': data.refreshToken
    }
  };
  try {
    let authResponse = await cognitoIdentityServiceProvider.initiateAuth(params).promise()

    let response = authResponse.AuthenticationResult
    response.Username = await getUsername(authResponse.AuthenticationResult.AccessToken)

    return success(response)
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const signup = async (data) => {
  let params = {
    ClientId: ClientId,
    Username: data.username,
    Password: data.password,
    UserAttributes: [
      {
        Name: 'nickname',
        Value: data.username
      },
      {
        Name: 'email',
        Value: data.email
      }
    ],
  };
  try {
    await cognitoIdentityServiceProvider.signUp(params).promise()
    return success({ username: data.username })
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const returnUsername = async (token) => {
  if (!token) return failure({ message: 'Missing access token' })
  try {
    let username = await getUsername(token)
    return success({ username })
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const changePassword = async (data) => {
  if (!data.accessToken) return failure({ message: 'Missing access token' })

  let params = {
    AccessToken: data.accessToken,
    PreviousPassword: data.oldPassword,
    ProposedPassword: data.newPassword
  };
  try {
    await cognitoIdentityServiceProvider.changePassword(params).promise()
    return success()
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const forgotPassword = async (data) => {
  let params = {
    ClientId: ClientId,
    Username: data.username
  };
  try {
    await cognitoIdentityServiceProvider.forgotPassword(params).promise()
    return success()
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const confirmForgotPassword = async (data) => {
  let params = {
    ClientId: ClientId,
    ConfirmationCode: data.confirmationCode,
    Username: data.username,
    Password: data.password,
  };
  try {
    await cognitoIdentityServiceProvider.confirmForgotPassword(params).promise()
    return success()
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}