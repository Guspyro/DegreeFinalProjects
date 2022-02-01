'use strict';
const { success, failure } = require('./common/responses.js')
const axios = require('axios');
const path = require('path');
const AWS = require('aws-sdk')
AWS.config.region = 'eu-west-3';

const BaseUrlAuth = "https://4njsldxxh0.execute-api.eu-west-3.amazonaws.com/prod/"
const AWSAccessKey = ''
const AWSSecretKey = ''
const bucketName = 'game-pcg-bucket'
const fileExtension = '.world'

var s3 = new AWS.S3({ accessKeyId: AWSAccessKey, secretAccessKey: AWSSecretKey });

exports.handler = async (event) => {
  const data = event.hasOwnProperty('body') ? event.body : undefined
  const token = event.queryStringParameters.accessToken
  try {
    switch (event.httpMethod) {
      case "GET":
        if (event.path.endsWith('worlds')) {
          return listWorlds(token)
        } else if (event.path.endsWith('world')) {
          return getWorld(token, event.queryStringParameters.worldName)
        }
        return failure({ message: 'Wrong GET url' })
      case "PUT":
        if (event.path.endsWith('world')) {
          return uploadWorld(token, event.queryStringParameters.worldName, data)
        }
        return failure({ message: 'Wrong PUT url' })
      case "DELETE":
        if (event.path.endsWith('world')) {
          return deleteWorld(token, event.queryStringParameters.worldName)
        }
        return failure({ message: 'Wrong PUT url' })
      default:
        return success({ message: 'The request type is not defined' })
    }
  } catch (error) {
    console.error('Error', error)
    return failure(error)
  }
}

const getPrefix = async (token) => {
  try {
    let response = await axios.get(BaseUrlAuth + 'username?accessToken=' + token)
    return response.data.username + '/worlds/'
  } catch (err) {
    console.error(err)
  }
}

const listWorlds = async (token) => {
  try {
    let prefix = await getPrefix(token)
    let params = {
      Bucket: bucketName,
      Prefix: prefix
    };
    let response = await s3.listObjects(params).promise()
    let worlds = []
    response.Contents.forEach((world, index) => {
      worlds.push(path.parse(world.Key).name)
    })
    return success({ worlds: JSON.stringify(worlds) })
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const getWorld = async (token, worldName) => {
  if (!worldName) return failure('Missing World Name')
  try {
    let prefix = await getPrefix(token)
    let params = {
      Bucket: bucketName,
      Key: prefix + worldName + fileExtension
    };
    let response = await s3.getObject(params).promise()
    let world = Buffer.from(response.Body).toString()
    return success({ world })
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const uploadWorld = async (token, worldName, world) => {
  if (!world) return failure({ message: 'Missing World' })
  if (!worldName) return failure({ message: 'Missing World Name' })

  try {
    let prefix = await getPrefix(token)
    let params = {
      Bucket: bucketName,
      Key: prefix + worldName + fileExtension,
      Body: world
    };
    await s3.putObject(params).promise()
    return success({ worldName })
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}

const deleteWorld = async (token, worldName) => {
  if (!worldName) return failure('Missing World Name')
  try {
    let prefix = await getPrefix(token)
    let params = {
      Bucket: bucketName,
      Key: prefix + worldName + fileExtension
    };
    await s3.deleteObject(params).promise()
    return success({ worldName })
  } catch (error) {
    console.error(error)
    return failure(error)
  }
}