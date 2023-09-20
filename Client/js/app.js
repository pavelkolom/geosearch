'use strict';

(function () {
    function init() {
        var router = new Router([
            new Route('byip', 'byip.html', true),            
            new Route('byname', 'byname.html')
        ]);
    }
    init();
}());
