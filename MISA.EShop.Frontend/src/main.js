import Vue from 'vue'
import App from './App.vue'

import VueRouter from "vue-router";
import axios from 'axios'
import VueAxios from 'vue-axios'
import SlideUpDown from 'vue-slide-up-down';
import ProductList from "./view/product/ProductList.vue";

Vue.use(VueRouter);
Vue.use(VueAxios, axios)
Vue.config.productionTip = false
Vue.component('slide-up-down', SlideUpDown)

const routes = [
    { path: '/product', name: 'Product', component: ProductList },
    { path: '', redirect: '/product' },
]

const router = new VueRouter({
    mode: 'history',
    routes,
})

new Vue({
    router,
    render: h => h(App),
}).$mount('#app')