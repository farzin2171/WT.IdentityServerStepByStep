var config = {
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44378/",
    client_id: "client_id_js",
    response_type: "id_token token",
    redirect_uri: "https://localhost:44315/Home/signIn",
    scope: "openid ApiOne ApiTwo wt.scope"
};


var userManager = new Oidc.UserManager(config);


var signIn = function () {
    userManager.signinRedirect();

}

userManager.getUser().then(user => {

    console.log("user:", user);
    if (user) {
        console.log('axios');
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
    }
});

var callApi = function () {
    axios.get("https://localhost:44328/Secret")
        .then(res => {
            console.log(res);
        });
}

var refreshing = false;
axios.interceptors.response.use(
    function (response) { return response; },
    function (error) {
        console.log("axios error", error.response);
        var axiosConfig = error.response.config;
        //if error response is 401 try the rfersh token
        if (error.response.status === 401) {
            if (!refreshing) {
                refreshing = true;
                return userManager.signinSilent().then(user => {
                    console.log(user);
                    //update the http client and request
                    axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
                    axiosConfig.headers.common["Authorization"] = "Bearer " + user.access_token;
                    //retry the http request
                    axios(axiosConfig);
                });
            }
        }
        return Promise.reject(error);
    });

