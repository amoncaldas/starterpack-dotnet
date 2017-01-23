module.exports = {
  expectToastToEqual: expectToastToEqual,
  getModalYesButton: getModalYesButton,
  getModalNoButton: getModalNoButton
};

function expectToastToEqual( toastMessage ){
  var locator = by.css('.md-toast-text');
  var timeout = 3000;
  var EC = protractor.ExpectedConditions;

  browser.ignoreSynchronization = true;
  browser.wait(EC.visibilityOf(element( locator )), timeout).then(function () {
    expect(element( locator ).getText()).toEqual(toastMessage);
    browser.ignoreSynchronization = false;
 });
};

function getModalYesButton() {
  return element(by.css('button[ng-click="ctrl.yesAction()"]'));
}

function getModalNoButton() {
  return element(by.css('button[ng-click="ctrl.noAction()"]'));
}
