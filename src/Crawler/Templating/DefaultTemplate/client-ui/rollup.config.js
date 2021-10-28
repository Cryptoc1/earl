import alias from '@rollup/plugin-alias'
import babel from '@rollup/plugin-babel'
import commonjs from '@rollup/plugin-commonjs'
import json from '@rollup/plugin-json'
import resolve from '@rollup/plugin-node-resolve'
import autoprefixer from 'autoprefixer'
import path from 'path'
import postcssUrl from 'postcss-url'
import clean from 'rollup-plugin-cleaner'
import postcss from 'rollup-plugin-postcss'
import { terser } from 'rollup-plugin-terser'

const production = !process.env.ROLLUP_WATCH

const configureBundle = (name, entryFile) => {
  const workingDir = process.cwd()
  const srcDir = path.resolve(workingDir, './src')
  const buildDir = path.resolve(workingDir, '../wwwroot/dist')

  const source = srcPath => path.resolve(srcDir, srcPath)
  const dist = srcPath => path.resolve(buildDir, srcPath)

  if (!entryFile) entryFile = name + '.js'
  return {
    input: `src/${entryFile}`,
    output: {
      file: `../wwwroot/dist/${name}.min.js`,
      format: 'cjs',
      plugins: [terser()],
      sourcemap: true
    },
    plugins: [
      clean({ targets: [`../wwwroot/dist/${name}.min.js`, `../wwwroot/dist/${name}.min.css`] }),
      resolve({
        browser: true,
      }),
      json(),
      commonjs(),
      babel({
        babelrc: false,
        babelHelpers: 'runtime',
        exclude: [/\/core-js\//],
        plugins: ['@babel/plugin-transform-runtime'],
        presets: [
          [
            '@babel/preset-env',
            {
              corejs: 3,
              useBuiltIns: 'entry',
              targets: '> 0.25%, not dead, not ie 11, not op_mini all'
            }
          ]
        ]
      }),
      postcss({
        extract: true,
        inject: false,
        minimize: true,
        plugins: [
          autoprefixer,
          postcssUrl({
            assetsPath: dist('assets'),
            basePath: source('styles'),
            fallback: 'custom',
            maxSize: 64,
            optimizeSvgEncode: true,
            url: 'inline'
          }),
          postcssUrl({
            assetsPath: dist('assets'),
            basePath: source('styles'),
            hashOptions: {
              append: true
            },
            url: 'copy',
            useHash: true
          }),
          postcssUrl({
            url: (asset, dir) => {
              // return data uris transform via inline options
              if (
                /^data:((?:\w+\/(?:(?!;).)+)?)((?:;[\w-=]*[^;])*),(.+)$/.test(
                  asset.url
                )
              ) {
                return asset.url
              }

              return `assets/${path.basename(asset.absolutePath)}${
                asset.search
              }${asset.hash}`
            }
          })
        ],
        sourceMap: !production
      }),
      alias({
        entries: {
          '~': `${srcDir}`,
          '~styles': source('styles')
        }
      })
    ],
    watch: { exclude: 'node_modules/**' }
  }
}

export default [
  configureBundle('master', 'index.js')
]
