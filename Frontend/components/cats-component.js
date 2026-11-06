import { catsUrl, tokenName, getStoredUserInfo, getErrorMessage } from "../app-config.js";

export default {
    template: `
        <div class="registry-shell">
            <header class="topbar">
                <a class="wordmark" href="#/" aria-label="Cat Registry home">
                    <span class="wordmark-mark" aria-hidden="true">C</span>
                    <span>CATALOG</span>
                </a>
                <div class="account-actions">
                    <span class="account-label">{{ userInfo.username }}</span>
                    <span class="role-label">{{ userInfo.role }}</span>
                    <button class="button-quiet" type="button" @click="logout">Sign out</button>
                </div>
            </header>

            <main class="registry-main">
                <section class="registry-heading">
                    <div class="heading-copy">
                        <p class="eyebrow">YOUR COLLECTION</p>
                        <h1>Cat registry</h1>
                        <p class="heading-summary">A record of every member of the family.</p>
                    </div>
                    <div class="heading-photo">
                        <img src="https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba?auto=format&fit=crop&w=1000&q=80"
                            alt="A curious cat looking toward the camera">
                        <span class="photo-count"><strong>{{ cats.length }}</strong> {{ cats.length === 1 ? "cat" : "cats" }}</span>
                    </div>
                </section>

                <section v-if="userInfo.role === 'Admin'" class="add-cat-section">
                    <div class="add-cat-heading">
                        <p class="eyebrow">QUICK ACTION</p>
                        <h2>Add a cat</h2>
                    </div>
                    <form class="add-cat-form" @submit.prevent="addCat">
                        <div class="field-group">
                            <label for="cat-name">Name</label>
                            <input id="cat-name" v-model="newCat.name" placeholder="e.g. Miso" required>
                        </div>
                        <div class="field-group weight-field">
                            <label for="cat-weight">Weight</label>
                            <div class="input-suffix">
                                <input id="cat-weight" v-model.number="newCat.weight" type="number" min="0" step="any" required>
                                <span>kg</span>
                            </div>
                        </div>
                        <button class="button-primary add-button" type="submit">
                            <span aria-hidden="true">+</span> Add cat
                        </button>
                    </form>
                </section>

                <section class="cats-section">
                    <div class="section-heading">
                        <div>
                            <p class="eyebrow">THE ROSTER</p>
                            <h2>All cats</h2>
                        </div>
                        <span class="section-count">{{ cats.length }} TOTAL</span>
                    </div>
                    <div v-if="cats.length > 0" class="table-frame">
                        <table class="cat-table">
                            <thead>
                                <tr><th>Name</th><th>Weight</th><th class="action-column">Actions</th></tr>
                            </thead>
                            <tbody>
                                <tr v-for="cat in cats" :key="cat.id">
                                    <td class="cat-name">{{ cat.name }}</td>
                                    <td>{{ cat.weight }} <span class="weight-unit">kg</span></td>
                                    <td class="action-column">
                                        <button :disabled="userInfo.role !== 'Admin'" class="button-delete"
                                            type="button" @click="deleteCat(cat.id)">Delete</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div v-else class="empty-state">
                        <span class="empty-mark" aria-hidden="true">C</span>
                        <h3>No cats in the registry yet</h3>
                        <p v-if="userInfo.role === 'Admin'">Add the first cat using the form above.</p>
                    </div>
                </section>
                <p v-if="message" class="status-message" :class="{ 'is-error': message.startsWith('Failed') }" role="status">
                    {{ message }}
                </p>
            </main>
        </div>
    `,
    data() {
        return {
            userInfo: getStoredUserInfo(),
            cats: [],
            message: null,
            newCat: { name: "", weight: null }
        };
    },
    created() {
        this.getCats();
    },
    methods: {
        logout() {
            localStorage.removeItem(tokenName);
            this.$router.replace({ name: "login" });
        },
        getCats() {
            axios.get(catsUrl)
                .then(response => {
                    this.cats = response.data;
                    this.message = "Fetched cats successfully";
                })
                .catch(ex => {
                    this.message = "Failed to fetch cats: " + getErrorMessage(ex);
                });
        },
        addCat() {
            axios.post(catsUrl, this.newCat)
                .then(response => {
                    this.cats.push(response.data);
                    this.message = "Cat added successfully";
                    this.newCat = { name: "", weight: null };
                })
                .catch(ex => {
                    this.message = "Failed to add cat: " + getErrorMessage(ex);
                });
        },
        deleteCat(catId) {
            axios.delete(`${catsUrl}/${catId}`)
                .then(() => {
                    this.cats = this.cats.filter(cat => cat.id !== catId);
                    this.message = "Cat deleted successfully";
                })
                .catch(ex => {
                    this.message = "Failed to delete cat: " + getErrorMessage(ex);
                });
        }
    }
};