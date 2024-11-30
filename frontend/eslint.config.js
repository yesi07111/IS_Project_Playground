/**
 * Configuración de ESLint para un proyecto TypeScript con React.
 *
 * Este archivo configura ESLint, una herramienta de análisis estático de código,
 * para un proyecto que utiliza TypeScript y React. ESLint ayuda a identificar y
 * corregir problemas en el código, asegurando buenas prácticas de codificación.
 * La configuración integra plugins y configuraciones recomendadas, incluyendo
 * soporte para manejar hooks de React y el refresco de componentes.
 *
 * El refresco de componentes es una característica que permite actualizar
 * componentes de React en tiempo real sin perder su estado, mejorando la
 * experiencia de desarrollo al proporcionar un feedback inmediato.
 *
 * - **ignores**: Excluye la carpeta 'dist' del análisis.
 * - **extends**: Extiende las configuraciones recomendadas de ESLint y TypeScript.
 * - **files**: Aplica la configuración a archivos con extensiones .ts y .tsx.
 * - **languageOptions**: Define la versión de ECMAScript y los globales del navegador.
 * - **plugins**: Incluye plugins para manejar hooks de React y el refresco de componentes.
 * - **rules**: Define reglas específicas, como advertencias para exportaciones de componentes.
 */
import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import tseslint from 'typescript-eslint'

export default tseslint.config(
  { ignores: ['dist'] },
  {
    extends: [js.configs.recommended, ...tseslint.configs.recommended],
    files: ['**/*.{ts,tsx}'],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
    plugins: {
      'react-hooks': reactHooks,
      'react-refresh': reactRefresh,
    },
    rules: {
      ...reactHooks.configs.recommended.rules,
      'react-refresh/only-export-components': [
        'warn',
        { allowConstantExport: true },
      ],
    },
  },
)