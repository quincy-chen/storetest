'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

//开发环境配置
module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  API_ROOT2: '"http://localhost:42095/api"',
  API_ROOT: '"http://localhost:42095/api"'
})
