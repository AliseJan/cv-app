$(document).ready(function () {
    $.validator.setDefaults({
        errorClass: 'text-danger',
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.insertAfter(element);
        }
    });

    $("#input-form").validate();

    $(".add-school-button").click(function () {
        var schoolTemplate = $("#school-template").html();
        var newIndex = $(".school-entry").length;
        schoolTemplate = schoolTemplate.replace(/\[i\]/g, "[" + newIndex + "]");
        var $newSchoolSection = $(schoolTemplate);
        $("#school-section").append($newSchoolSection);

    });

    $(".remove-school-button").click(function () {
        if ($(".school-entry").length > 1) {
            $(".school-entry").last().remove();
        } else {
            alert("At least one school entry is required");
        }
    });

    $(".add-job-button").click(function () {
        var jobTemplate = $("#job-template").html();
        var newIndex = $(".job-entry").length;
        jobTemplate = jobTemplate.replace(/\[i\]/g, "[" + newIndex + "]");
        var $newJob = $(jobTemplate);
        $newJob.data("index", newIndex);
        $("#job-section").append($newJob);
    });

    $(".remove-job-button").click(function () {
        if ($(".job-entry").length > 1) {
            $(".job-entry").last().remove();
        } else {
            alert("At least one job entry is required");
        }
    });

    $("#job-section").on("click", ".add-skill-button", function () {
        var skillTemplate = $("#skill-template").html();
        var jobIndex = $(this).closest(".job-entry").data("index");
        var newIndex = $(this).closest(".job-entry").find(".skills-container .mb-3").length;
        skillTemplate = skillTemplate.replace(/\[i\]/g, "[" + jobIndex + "]");
        skillTemplate = skillTemplate.replace(/\[j\]/g, "[" + newIndex + "]");
        $(this).closest(".job-entry").find(".skills-container .mb-3:last").append(skillTemplate);
    });

    $("#job-section").on("click", ".remove-skill-button", function () {
        var numSkillSections = $(this).closest(".job-entry").find(".skills-container .mb-3").length;
        if (numSkillSections > 1) {
            $(this).closest(".job-entry").find(".skills-container .mb-3:last").remove();
        } else {
            alert("At least one skill section is required.");
        }
    });

    $("#job-section").on("click", ".add-achievement-button", function () {
        var achievementTemplate = $("#achievement-template").html();
        var jobIndex = $(this).closest(".job-entry").data("index");
        var newIndex = $(this).closest(".job-entry").find(".achievements-container .mb-3").length;
        achievementTemplate = achievementTemplate.replace(/\[i\]/g, "[" + jobIndex + "]");
        achievementTemplate = achievementTemplate.replace(/\[j\]/g, "[" + newIndex + "]");
        $(this).closest(".job-entry").find(".achievements-container .mb-3:last").append(achievementTemplate);
    });

    $("#job-section").on("click", ".remove-achievement-button", function () {
        var numAchievementSections = $(this).closest(".job-entry").find(".achievements-container .mb-3").length;
        if (numAchievementSections > 1) {
            $(this).closest(".job-entry").find(".achievements-container .mb-3:last").remove();
        } else {
            alert("At least one achievement section is required.");
        }
    });
});