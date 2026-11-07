export const authUrl = "http://localhost:5240/api/Auth/login";
export const catsUrl = "https://anbo-restcatjwt.azurewebsites.net/api/Cats";
//export const catsUrl = "https://anbo-restcatjwt.azurewebsites.net/api/Cats";
export const tokenName = "userInfo";

export function getStoredUserInfo() {
    try {
        const userInfo = JSON.parse(localStorage.getItem(tokenName));
        return userInfo && userInfo.token ? userInfo : null;
    } catch {
        localStorage.removeItem(tokenName);
        return null;
    }
}

export function getErrorMessage(error) {
    const body = error.response?.data;

    if (typeof body === "string" && body.trim()) {
        return body;
    }
    if (body && typeof body === "object") {
        const message = body.message || body.detail || body.title || body.error;
        if (typeof message === "string" && message.trim()) {
            return message;
        }
        if (body.errors && typeof body.errors === "object") {
            const validationMessages = Object.values(body.errors).flat();
            if (validationMessages.length > 0) {
                return validationMessages.join(" ");
            }
        }
    }

    if (!error.response) {
        return "No response from the API. Check that it is running and allows this site's origin through CORS.";
    }

    return error.message || "An unexpected error occurred.";
}