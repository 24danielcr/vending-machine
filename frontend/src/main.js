import { createApp } from 'vue'
import App from './App.vue'

import { createRouter, createWebHistory } from "vue-router";

import HomePage from "./components/HomePage.vue";

const router = createRouter({
	history: createWebHistory(),
	routes: [
		{path: "/", name: "HomePage", component: HomePage},
	],
});

createApp(App).use(router).mount('#app');
