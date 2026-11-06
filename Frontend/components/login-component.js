import { authUrl, tokenName, getErrorMessage } from "../app-config.js";

export default {
    template: `
        <main class="login-layout">
            <section class="login-visual" aria-label="Cat portrait">
                <img src="https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba?auto=format&fit=crop&w=1600&q=85"
                    alt="A curious cat looking toward the camera">
                <div class="login-visual-caption">
                    <span class="eyebrow">CAT REGISTRY</span>
                    <p>Every cat has a place here.</p>
                </div>
            </section>
            <section class="login-panel">
                <a class="wordmark" href="#/" aria-label="Cat Registry home">
                    <span class="wordmark-mark" aria-hidden="true">C</span>
                    <span>CATALOG</span>
                </a>
                <div class="login-form-wrap">
                    <p class="eyebrow">MEMBER ACCESS</p>
                    <h1>Welcome back.</h1>
                    <form class="login-form" @submit.prevent="login">
                        <div class="field-group">
                            <label for="username">Username</label>
                            <input id="username" v-model="auth.username" autocomplete="username"
                                placeholder="Your username" type="text" required>
                        </div>
                        <div class="field-group">
                            <label for="password">Password</label>
                            <input id="password" v-model="auth.password" autocomplete="current-password"
                                placeholder="Your password" type="password" required>
                        </div>
                        <button class="button-primary login-submit" type="submit" :disabled="isLoggingIn">
                            {{ isLoggingIn ? "Signing in..." : "Sign in" }}
                            <span aria-hidden="true">&rarr;</span>
                        </button>
                    </form>
                    <p v-if="authMessage" class="status-message is-error" role="alert">{{ authMessage }}</p>
                </div>
                <p class="login-footer">CATALOG <span>·</span> PRIVATE COLLECTION</p>
            </section>
        </main>
    `,
    data() {
        return {
            auth: { username: "", password: "" },
            authMessage: null,
            isLoggingIn: false
        };
    },
    methods: {
        async login() {
            this.isLoggingIn = true;
            this.authMessage = null;

            try {
                const response = await axios.post(authUrl, this.auth);
                const userInfo = {
                    username: this.auth.username,
                    role: response.data.role,
                    token: response.data.token
                };
                localStorage.setItem(tokenName, JSON.stringify(userInfo));
                await this.$router.replace({ name: "cats" });
            } catch (ex) {
                this.authMessage = "Authentication failed: " + getErrorMessage(ex);
                this.isLoggingIn = false;
            }
        }
    }
};