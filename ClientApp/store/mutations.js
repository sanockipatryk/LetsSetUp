export const showAuthModal = state => {
    state.showAuthModal = true;
};
export const hideAuthModal = state => {
    state.showAuthModal = false;
};
export const loginRequest = state => {
    state.loading = true;
};
export const loginSuccess = (state, payload) => {
    state.auth = payload;
    state.loading = false;
    state.showAuthModal = false;
};
export const updateAuthSuccess = (state, auth) => {
    state.auth = auth;
};
export const loginError = state => {
    state.loading = false;
};
export const registerRequest = state => {
    state.loading = true;
};
export const registerSuccess = state => {
    state.loading = false;
};
export const registerError = state => {
    state.loading = false;
};
export const logout = state => {
    state.auth = null;
};
export const initialise = (state, payload) => {
    Object.assign(state, payload);
};