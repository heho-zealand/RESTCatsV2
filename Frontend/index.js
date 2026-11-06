import LoginComponent from "./components/login-component.js";
import CatsComponent from "./components/cats-component.js";
import { catsUrl, getStoredUserInfo } from "./app-config.js";

/*
const catsApiUrl = new URL(catsUrl);
const catsApiPath = catsApiUrl.pathname.replace(/\/$/, "");

axios.interceptors.request.use(config => {
    const requestUrl = new URL(config.url, config.baseURL || window.location.href);
    const isCatsApiRequest = requestUrl.origin === catsApiUrl.origin &&
        (requestUrl.pathname === catsApiPath || requestUrl.pathname.startsWith(`${catsApiPath}/`));

    if (isCatsApiRequest) {
        const token = getStoredUserInfo()?.token;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
    }

    return config;
});
*/
axios.interceptors.request.use(config => {
    
        const token = getStoredUserInfo()?.token;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        
    }

    return config;
});

const router = VueRouter.createRouter({
    history: VueRouter.createWebHashHistory(),
    routes: [
        { path: "/", redirect: () => getStoredUserInfo() ? "/cats" : "/login" },
        { path: "/login", name: "login", component: LoginComponent },
        { path: "/cats", name: "cats", component: CatsComponent, meta: { requiresAuth: true } }
    ]
});

router.beforeEach(to => {
    const isAuthenticated = Boolean(getStoredUserInfo());

    if (to.meta.requiresAuth && !isAuthenticated) {
        return { name: "login" };
    }
    if (to.name === "login" && isAuthenticated) {
        return { name: "cats" };
    }
});

Vue.createApp({}).use(router).mount("#app");