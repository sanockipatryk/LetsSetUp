export const isAuthenticated = state => {
    return (
        state.auth !== null &&
        state.auth.access_token !== null &&
        new Date(state.auth.access_token_expiration) > new Date()
    );
};
export const isInRole = (state, getters) => role => {
    const result = getters.isAuthenticated && state.auth.roles.indexOf(role) > -1;
    return result;
};