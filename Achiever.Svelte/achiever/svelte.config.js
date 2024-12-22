import azure from 'svelte-adapter-azure-swa';
import { vitePreprocess } from '@sveltejs/vite-plugin-svelte';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	preprocess: vitePreprocess(), // Enables preprocessing (e.g., TypeScript, PostCSS)

	kit: {
		adapter: azure(), // Default adapter, works for most environments
		alias: {
			// Shorten import paths for common directories
			$lib: './src/lib',
			$components: './src/lib/components',
			$stores: './src/lib/stores',
		},
	},

	vitePlugin: {
		experimental: {
			inspector: true, // Optional: enables SvelteKit's inspector for dev debugging
		},
	},
};

export default config;