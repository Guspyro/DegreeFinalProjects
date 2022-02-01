'use strict'
const success = (body) => {
    return buildResponse(200, body)
}

const failure = (body) => {
    return buildResponse(500, body)
}

const buildResponse = (statusCode, body) => {
    return {
        statusCode: statusCode,
        body: JSON.stringify(body)
    }
}

module.exports = {
    success,
    failure
}
