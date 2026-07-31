import js from '@eslint/js';
import pluginVue from 'eslint-plugin-vue';
import globals from 'globals';

export default [
  js.configs.recommended,
  ...pluginVue.configs['flat/essential'],
  {
    ignores: ['dist/**', 'node_modules/**', '.vite/**']
  },
  {
    languageOptions: {
      globals: {
        ...globals.browser,
        ...globals.node,
        ...globals.es2021
      }
    },
    rules: {
      'no-unused-vars': 'off',
      'no-empty': 'off',
      'vue/multi-word-component-names': 'off'
    }
  }
];
